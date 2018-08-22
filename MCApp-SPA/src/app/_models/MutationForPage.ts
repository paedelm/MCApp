import { AccountForDetailed } from './AccountForDetailed';
import { MutationForList } from './MutationForList';

export interface MutationForPage {
    account: AccountForDetailed;
    mutations: MutationForList[];
}
