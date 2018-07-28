import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { User } from '../_models/User';
import { PaginatedResult } from '../_models/Pagination';
import { Message } from '../_models/message';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { UserWithAccounts } from '../_models/UserWithAccounts';
import { Account } from '../_models/Account';
import { AccountForDetailed } from '../_models/AccountForDetailed';
import { Mutation } from '../_models/Mutation';
import { MutationForDetailed } from '../_models/MutationForDetailed';

@Injectable()
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private authHttp: HttpClient) {}

  getAccounts(userId): Observable<UserWithAccounts> {
    return this.authHttp.get<UserWithAccounts>(this.baseUrl + 'users/' + userId + '/accounts');
  }

  getUsers(
    page?,
    itemsPerPage?,
    userParams?: any,
    likesParam?: string
  ): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<
      User[]
    >();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    if (likesParam === 'Likers') {
      params = params.append('Likers', 'true');
    }
    if (likesParam === 'Likees') {
      params = params.append('Likees', 'true');
    }
    if (userParams != null) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }

    return this.authHttp
      .get<User[]>(this.baseUrl + 'users', { observe: 'response', params })
      .pipe(
        map((rsp: HttpResponse<User[]>) => {
          paginatedResult.result = rsp.body;
          if (rsp.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(
              rsp.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  addAccount(userId: number, account: Account): Observable<AccountForDetailed> {
    return this.authHttp.post<AccountForDetailed>(
      this.baseUrl + 'users/' + userId + '/accounts',
      account);
  }
  getUser(id): Observable<User> {
    return this.authHttp.get<User>(this.baseUrl + 'users/' + id);
  }

  addMutation(userId: number, account: Account, mutation: Mutation): Observable<MutationForDetailed> {
    mutation.accountname = account.accountname;
    return this.authHttp.post<MutationForDetailed>(
      this.baseUrl + 'users/' + userId + '/accounts/' + account.id + '/mutations',
      mutation);
  }

  updateUser(id: number, user: User) {
    return this.authHttp.put(this.baseUrl + 'users/' + id, user);
  }

  setMainPhoto(userId: number, id: number) {
    return this.authHttp.post(
      this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain',
      {}
    );
  }

  deletePhoto(userId: number, id: number) {
    return this.authHttp.delete(
      this.baseUrl + 'users/' + userId + '/photos/' + id
    );
  }

  sendLike(id: number, recipientId: number) {
    return this.authHttp.post(
      this.baseUrl + 'users/' + id + '/like/' + recipientId,
      {}
    );
  }

  getMessages(id: number, page?, itemsPerPage?, messageContainer?: string) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<
      Message[]
    >();
    let params = new HttpParams();
    if (messageContainer) {
      params = params.append('MessageContainer', messageContainer);
    }
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.authHttp
      .get<Message[]>(this.baseUrl + 'users/' + id + '/messages', {
        observe: 'response',
        params: params
      })
      .pipe(
          map((rsp: HttpResponse<Message[]>) => {
            paginatedResult.result = rsp.body;
            if (rsp.headers.get('Pagination') != null) {
                paginatedResult.pagination = JSON.parse(
                rsp.headers.get('Pagination')
                );
            }
            return paginatedResult;
            })
        );
  }

  getMessageThread(id: number, recipientId: number) {
    return this.authHttp.get<Message[]>(
      this.baseUrl + 'users/' + id + '/messages/thread/' + recipientId
    );
  }

  sendMessage(id: number, message: Message) {
    return this.authHttp.post<Message>(
      this.baseUrl + 'users/' + id + '/messages',
      message
    );
  }

  deleteMessage(id: number, userId: number) {
    return this.authHttp.post(
      this.baseUrl + 'users/' + userId + '/messages/' + id,
      {}
    );
  }

  markAsRead(id: number, userId: number) {
    return this.authHttp
      .post(this.baseUrl + 'users/' + userId + '/messages/' + id + '/read', {})
      .subscribe();
  }
}
