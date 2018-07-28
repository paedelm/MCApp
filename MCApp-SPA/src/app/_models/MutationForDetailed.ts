import { AccountForDetailed } from './AccountForDetailed';

export interface MutationForDetailed {
    id: number;
    account: AccountForDetailed;
    description: string;
    created: Date;
    amount: number;
    balance: number;
}
