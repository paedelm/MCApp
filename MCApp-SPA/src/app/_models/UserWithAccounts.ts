import { Photo } from './Photo';
export interface UserWithAccounts {
  id: number;
  username: string;
  knownAs: string;
  photoUrl?: string;
  city: string;
  country: string;
  interests?: string;
  introduction?: string;
  lookingFor?: string;
  photos?: Photo[];
  accounts: Account[];
  fromCache?: boolean;
}
