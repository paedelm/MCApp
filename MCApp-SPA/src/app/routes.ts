import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
// import { MessagesComponent } from './messages/messages.component';
// import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';
import { AccountListResolver } from './_resolvers/account-list.resolver';
import { AccountsComponent } from './accounts/accounts.component';
import { MutationsResolver } from './_resolvers/mutations.resolver';
import { MutationsComponent } from './mutations/mutations.component';
// import { MessagesResolver } from './_resolvers/messages.resolver';

export const appRoutes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full'},
    { path: 'home', component: HomeComponent },
    { path: '', runGuardsAndResolvers: 'always', canActivate: [AuthGuard],
        children: [
            { path: 'accounts', component: AccountsComponent, resolve: {user: AccountListResolver} },
            { path: 'members/:id', component: MemberDetailComponent, resolve: {user: MemberDetailResolver} },
            { path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver},
             canDeactivate: [PreventUnsavedChanges] },
            { path: 'mutations', component: MutationsComponent, resolve: { pgMutations: MutationsResolver} }
            // { path: 'lists', component: ListsComponent, resolve: {users: ListsResolver} }
        ]
    },
    { path: '**', redirectTo: 'home', pathMatch: 'full' }
];
