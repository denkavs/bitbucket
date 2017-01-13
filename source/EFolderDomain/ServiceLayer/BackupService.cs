using EFolderDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFolderDomain.Model;
using System.IO;
using EFolderDomain.Infrastructure;

namespace EFolderDomain.ServiceLayer
{
    public class BackupService : IBackupService
    {
        private IBackupRepository backupRepository;
        private IToDoItemFetcher fetcher;
        private IToDoRepository todoRepository;
        private IToDoItemStreamParser parser;
        private CircuitBreaker<Stream> breaker;

        public BackupService(IBackupRepository backup, IToDoItemFetcher fetcher, IToDoRepository todoRepository, IToDoItemStreamParser parser, CircuitBreaker<Stream> breaker) {
            this.backupRepository = backup;
            this.fetcher = fetcher;
            this.todoRepository = todoRepository;
            this.parser = parser;
            this.breaker = breaker;
        }

        public Task<IEnumerator<ToDoItem>> ExportToDoItems(int backUpId)
        {
            return Task<IEnumerator<ToDoItem>>.Factory.StartNew(() =>
            {
                Logger.Log.Info(string.Format("Getting list of ToDoItems for backupId-[0]", backUpId));
                return this.todoRepository.GetEnumerator(backUpId);
            });
        }

        public IEnumerable<Backup> ListBackup()
        {
            Logger.Log.Info("Getting list of backups...");
            return this.backupRepository.List();
        }

        public Task<Backup> MakeBackup()
        {
            Backup bk = null;
            Task<Backup> t = Task.Factory.StartNew(() =>
            {
                // add backup entity to repository
                bk = new Backup();
                bk.Date = DateTime.UtcNow;
                bk.Status = BackupStatus.InProgress;

                Backup newBackup = this.backupRepository.Add(bk);
                if (newBackup != null)
                {
                    try
                    {
                        int backId = newBackup.Id;
                        Logger.Log.Info(string.Format("Started new Backup. Id-[{0}]", backId));
                        Stream fetchResult = null;

                        fetchResult = this.breaker.ExecuteAction(() => { return this.fetcher.Fetch().Result; });

                        if (fetchResult == null)
                            bk.Status = BackupStatus.Failed;
                        else
                        {
                            // Parse from stream corresponding todoitem
                            this.parser.Init(fetchResult);
                            while (this.parser.Next())
                            {
                                ToDoItem item = this.parser.Current();
                                // save todoItem to repository
                                this.todoRepository.Add(item, backId);
                            }
                            ((IDisposable)this.parser).Dispose();
                        }
                        bk.Status = BackupStatus.Ok;
                    }
                    catch (CircuitBreakerOpenException ex)
                    {
                        bk.Status = BackupStatus.Failed;
                        Logger.Log.Error("Waiting timeout are not exceeded. Service are not available.", ex);
                    }
                    catch (Exception ex)
                    {
                        bk.Status = BackupStatus.Failed;
                        if(ex.InnerException != null)
                            Logger.Log.Error(ex.InnerException.Message, ex);
                        else
                            Logger.Log.Error(ex.Message, ex);
                    }
                }
                Logger.Log.Info(string.Format("Backup process for BackupID [{0}] has completed with status - {1}.", bk.Id, bk.Status));
                return bk;
            });

            return t;
        }
    }
}
