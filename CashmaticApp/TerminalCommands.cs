using hgi.Environment;
using SIX.TimApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SIX.TimApi.Terminal;

namespace CashmaticApp
{
    public class TerminalCommands
    {

        Terminal terminal;
        string reversableTrxRefNum = "";
        bool requestInProgress = false;

        public delegate void TerminalStatusChanged(object sender, TerminalStatus terminalStatus);
        public delegate void TerminalTransactionError(object sender, TimException exc);
        public delegate void TerminalTransactionCompleted(object sender, TransactionCompletedEventArgs EventArgs);

        public event TerminalStatusChanged StatusChanged;
        public event TerminalTransactionError TransactionError;
        public event TerminalTransactionCompleted TransactionCompleted;

        public TerminalCommands()
        {
            TerminalSettings settings = new TerminalSettings();
            settings.TerminalId = Global.terminalId;
            settings.LogDir = Global.terminalLog;

            terminal = new SIX.TimApi.Terminal(settings);
            terminal.TerminalStatusChanged += new Terminal.TerminalStatusChangedHandler(terminal_TerminalStatusChanged);
            terminal.TransactionCompleted += new Terminal.TransactionCompletedEventHandler(terminal_TransactionCompleted);
            terminal.BalanceCompleted += new Terminal.BalanceCompletedEventHandler(terminal_BalanceCompleted);
            terminal.PrintOptions.Cardholder.PrintWidth = Global.sixPrintReceiptWidth;

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
                    setReversableTransaction(transactionCompletedEventArgs.TransactionResponse.TransactionInformation.TransSeq);
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
            setRequestInProgress(false);
            if (balanceCompletedEventArgs.Error != null)
            {
                Debug.Log("CashmaticApp", balanceCompletedEventArgs.Error.ToString());
          
            }
        }
          
        void setReversableTransaction(string transactionReferenceNumber)
        {
            reversableTrxRefNum = transactionReferenceNumber;
           
            //btnReversal.Enabled = (!requestInProgress && transactionReferenceNumber != "");
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

        public void Dispose()
        {
            terminal.Dispose();
            Terminal.TimApiDispose();
        }
    }
}
