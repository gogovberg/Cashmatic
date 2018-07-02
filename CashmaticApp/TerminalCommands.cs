using hgi.Environment;
using SIX.TimApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static SIX.TimApi.Terminal;

namespace CashmaticApp
{
    public class TerminalCommands
    {

        Terminal terminal;
        string reversableTrxRefNum = "";
        bool requestInProgress = false;
        public bool isActivated = false;

        private Timer _balanceTimer;
        private object _balanceObject = new object();
        public delegate void TerminalStatusChanged(object sender, TerminalStatus terminalStatus);
        public delegate void TerminalTransactionError(object sender, TimException exc);
        public delegate void TerminalTransactionCompleted(object sender, TransactionCompletedEventArgs EventArgs);
        public delegate void TerminalActivateCompleted(object sender, ActivateCompletedEventArgs EventArgs);
        public delegate void TerminalDeactivateCompleted(object sender, DeactivateCompletedEventArgs EventArgs);

        public event TerminalStatusChanged StatusChanged;
        public event TerminalTransactionError TransactionError;
        public event TerminalTransactionCompleted TransactionCompleted;
        public event TerminalActivateCompleted ActivateCompleted;
        public event TerminalDeactivateCompleted DeactivateCompleted;

        public TerminalCommands()
        {
            TerminalSettings settings = new TerminalSettings();
            //settings.TerminalId = Global.terminalId;
            //settings.LogDir = Global.terminalLog;
            terminal = new SIX.TimApi.Terminal(settings);
            terminal.ActivateCompleted += new ActivateCompletedEventHandler(terminal_ActivateComplete);
            terminal.DeactivateCompleted += new DeactivateCompletedEventHandler(terminal_DeactivateComplete);
            terminal.TerminalStatusChanged += new Terminal.TerminalStatusChangedHandler(terminal_TerminalStatusChanged);
            terminal.TransactionCompleted += new Terminal.TransactionCompletedEventHandler(terminal_TransactionCompleted);
            terminal.BalanceCompleted += new Terminal.BalanceCompletedEventHandler(terminal_BalanceCompleted);
            terminal.PrintOptions.Cardholder.PrintWidth = Global.sixPrintReceiptWidth;


            _balanceTimer = new Timer();
            _balanceTimer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            _balanceTimer.Interval = Global.BalanceTimer;
            _balanceTimer.Enabled = true;
        }
        void terminal_ActivateComplete(object sender, SIX.TimApi.Terminal.ActivateCompletedEventArgs activateCompletedEventArgs)
        {
            if(activateCompletedEventArgs.Error==null)
            {
                isActivated = true;
                OnActivateComplete(activateCompletedEventArgs);
            }
            else
            {
                Debug.Log("CashmaticApp", activateCompletedEventArgs.TimError.ErrorMessage);
                OnTransactionError(activateCompletedEventArgs.TimError);
            }
           
        }
        void terminal_DeactivateComplete(object sender, SIX.TimApi.Terminal.DeactivateCompletedEventArgs deactivateCompletedEventArgs)
        {
            isActivated = false;
            OnDeactivateComplete(deactivateCompletedEventArgs);
        }
        void terminal_TerminalStatusChanged(object sender, SIX.TimApi.TerminalStatus trmStatus)
        {
            string response = "";
            foreach (string s in trmStatus.DisplayContent)
            {
                if(!string.IsNullOrEmpty(s))
                {
                    response += s + "\r";
                }

            }
            Debug.Log("CashmaticApp", response);
            OnTerminalstatusChanged(trmStatus);

        }
        void terminal_TransactionCompleted(object sender, SIX.TimApi.Terminal.TransactionCompletedEventArgs transactionCompletedEventArgs)
        {
            setRequestInProgress(false);
           
            // If event contains a null exception the transaction completed successfully.
            // Use data.getTransactionType() to see what kind of transaction finished if you
            // do not track this information yourself already. getTransactionType() is
            // present for your convenience.
            Debug.Log("CashmaticApp", "TransactionCompleted");
            if (transactionCompletedEventArgs.TimError == null)
            {
                if (transactionCompletedEventArgs.TransactionResponse.TransactionType == SIX.TimApi.Constants.TransactionType.Purchase)
                {
                    setReversableTransaction(transactionCompletedEventArgs.TransactionResponse.TransactionInformation.TransRef);
                    OnTransactionCompeted(transactionCompletedEventArgs);

                }
                else
                {
                    setReversableTransaction("");
                }
            }
            else
            {
                // If event contains an error the transaction failed. Show an error message.
                // The exception contains the error code and additional information if present.
                // The error message is provided in the exception.
            
                Debug.Log("CashmaticApp", transactionCompletedEventArgs.TimError.ErrorMessage);
                OnTransactionError(transactionCompletedEventArgs.TimError);


            }
        }
        void terminal_BalanceCompleted(object sender, Terminal.BalanceCompletedEventArgs balanceCompletedEventArgs)
        {
            Debug.Log("CashmaticApp","Balance completed");
            isActivated = false;
            setRequestInProgress(false);
            if (balanceCompletedEventArgs.Error != null)
            {
                Debug.Log("CashmaticApp", balanceCompletedEventArgs.Error.ToString());
            }
            else
            {
                if(balanceCompletedEventArgs.BalanceResponse!=null)
                {
                    if(balanceCompletedEventArgs.BalanceResponse.PrintData!=null)
                    {
                        try
                        {
                            string todaysDate = DateTime.Now.ToString("yyyyMMdd");
                            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
                            string merchantReceiptName = todaysDate + "_balance";
                            string merchantReceipt = string.Format("{0}Balances\\{1}.txt", baseDir, merchantReceiptName);
                            System.IO.File.WriteAllText(merchantReceipt, balanceCompletedEventArgs.BalanceResponse.PrintData.MerchantReceipt);
                        }
                        catch(Exception ex)
                        {
                            Debug.Log("CashmaticApp", ex.ToString());
                        }
                    }
                }
            }
        }
        void setReversableTransaction(string transactionReferenceNumber)
        {
            reversableTrxRefNum = transactionReferenceNumber;
           
            //btnReversal.Enabled = (!requestInProgress && transactionReferenceNumber != "");
        }
        public void onDeactivate()
        {
            try
            {
                Debug.Log("CashmaticApp", "onDeactivate");
                terminal.DeactivateAsync();

            }
            catch(SIX.TimApi.TimException exc)
            {
                setRequestInProgress(false);
                Debug.Log("CashmaticApp", exc.ToString());
                OnTransactionError(exc);
            }
        }
        public void onActivate()
        {
            try
            {
                Debug.Log("CashmaticApp", "onActivate");
                terminal.ActivateAsync();

            }
            catch (SIX.TimApi.TimException exc)
            {
                setRequestInProgress(false);
                Debug.Log("CashmaticApp", exc.ToString());
                OnTransactionError(exc);
            }
        }   
        /**
	    * Do a purchase transaction.
	    */
        public void onPurchase(double amount)
        {
            try
            {
                Debug.Log("CashmaticApp", "onPurchase "+ amount);
          
                //logger.info("Begin purchase");
                setRequestInProgress(true);
                // Clear the transaction data. It is used for reversal but not purchase
                terminal.TransactionData = null;
                // Run the transaction. Once completed the event transactionCompleted is raised.
                terminal.TransactionAsync(SIX.TimApi.Constants.TransactionType.Purchase, new SIX.TimApi.Amount(System.Convert.ToDecimal(amount), "EUR"));
            }
            catch (SIX.TimApi.TimException exc)
            {
                setRequestInProgress(false);
                Debug.Log("CashmaticApp", exc.ToString());
                OnTransactionError(exc);
            }
        }
        /**
	     * Cancel running transaction.
	     */
        public void onCancel()
        {
            try
            {
                Debug.Log("CashmaticApp", "onCancel");
                terminal.Cancel();
            }
            catch (TimException e)
            {
                Debug.Log("CashmaticApp", e.ToString());
                OnTransactionError(e);
            }
        }
        /**
	     * Do a reversal transaction.
	     */
        public void onReversal(double amount)
        {
            try
            {
                Debug.Log("CashmaticApp", "onReversal");
                setRequestInProgress(true);

                // Set the transaction data. Reversal needs the transaction reference number
                // of the previous transaction
                TransactionData trxData = new TransactionData();
                trxData.TransSeq = reversableTrxRefNum;
                terminal.TransactionData = trxData;

                // Run the transaction. Once completed the listener receives a
                // transactionCompleted notification
                terminal.TransactionAsync(SIX.TimApi.Constants.TransactionType.Reversal, new Amount(System.Convert.ToDecimal(amount), "EUR"));

            }
            catch (TimException e)
            {
                setRequestInProgress(false);
                Debug.Log("CashmaticApp", e.ToString());
                OnTransactionError(e);
            }
        }
        /**
	     * Do a balance.
	     */
        public void onBalance()
        {
            try
            {
                Debug.Log("CashmaticApp", "onBalance");
                setRequestInProgress(true);
                terminal.BalanceAsync();
            }
            catch (TimException e)
            {
                setRequestInProgress(false);
                Debug.Log("CashmaticApp", e.ToString());
                OnTransactionError(e);
            }
        }
        /**
	     * Set if a transaction or balance request is running. If true purchase, balance
	     * and reversal buttons are disabled and cancel button enabled.
	     */
        private void setRequestInProgress(bool inProgress)
        {
            requestInProgress = inProgress;

        }

        private void OnTransactionError(TimException e)
        {
            if (TransactionError != null)
            {
                TransactionError(this, e);
            }
        }
        private void OnTransactionCompeted(TransactionCompletedEventArgs EventArgs)
        {
            if (TransactionCompleted != null)
            {
                TransactionCompleted(this, EventArgs);
            }
        }
        private void OnTerminalstatusChanged(TerminalStatus status)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this, status);
            }
        }
        private void OnActivateComplete(ActivateCompletedEventArgs EventArgs)
        {
            if(ActivateCompleted!=null)
            {
                ActivateCompleted(this, EventArgs);
            }
        }
        private void OnDeactivateComplete(DeactivateCompletedEventArgs EventArgs)
        {
            if (DeactivateCompleted != null)
            {
                DeactivateCompleted(this, EventArgs);
            }
        }
        public void Dispose()
        {
            terminal.Dispose();
            Terminal.TimApiDispose();
            if(_balanceTimer!=null)
            {
                _balanceTimer.Enabled = false;
            }
            
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            lock (_balanceObject)
            {
                try
                {
                    DateTime balanceFrom = DateTime.Now.Date.AddHours(Global.BalanceTimeFrom);
                    DateTime balanceTo = DateTime.Now.Date.AddHours(Global.BalanceTimeTo);

                    if(DateTime.Compare(DateTime.Now,balanceFrom)>=0 &&
                       DateTime.Compare(DateTime.Now,balanceTo)<=0)
                    {
                        string todaysDate = DateTime.Now.ToString("yyyyMMdd");
                        string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
                        string merchantReceiptName = todaysDate + "_balance";
                        string merchantReceipt = string.Format("{0}Balances\\{1}.txt", baseDir, merchantReceiptName);
                        if(!File.Exists(merchantReceipt))
                        {
                            onBalance();

                        }
                        else
                        {
                            Debug.Log("CashmaticApp", string.Format("Balance {0} already exists.", merchantReceipt));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("CashmaticApp", ex.ToString());
                }
            }
        }

    }
}
