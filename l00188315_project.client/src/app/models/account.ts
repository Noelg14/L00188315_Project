
import { Balance } from "./balance";
import { Transaction } from "./transaction";
export interface Account {
    accountId: string;
    name: string;
    currency: string;
    accountType: string;
    accountSubType: string;
    iban: string;
    sortCode: string;
    balance: Balance | null;
    transactions: Transaction[] | null;
    created: string;
    updated: string;
    userId: string | null;
}
