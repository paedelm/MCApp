import { UserForAccount } from './UserForAccount';

export interface AccountForDetailed {
    id: number;
    user: UserForAccount;
    accountname: string;
    description: string;
    percentage: number;
    balance: number;
    CalculatedInterest: number;
    created: Date;
    lastActive: Date;
}
