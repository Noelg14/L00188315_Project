export interface Transaction {
  accountId: string
  amount: string
  amountCurrency: string
  creditDebitIndicator: string | null
  bookingDateTime: string
  creditorAccount: string
  debtorAccount: string
  proprietaryBankTransactionCode: string | null
  status: string
  transactionId: string
  transactionInformation: string | null
  userComments: string | null
}
