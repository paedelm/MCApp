import { User } from './User';
import { UserWithAccounts } from './UserWithAccounts';

export interface AuthUser {
    tokenString: string;
    user: UserWithAccounts;
}
