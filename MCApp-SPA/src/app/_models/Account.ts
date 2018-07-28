export interface Account {
    id: number;
    accountname: string;
    description: string;
    percentage: number;
    balance: number;
    CalculatedInterest: number;
    created: Date;
    lastActive: Date;
}
