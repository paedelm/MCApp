import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { User } from '../_models/User';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthUser } from '../_models/AuthUser';
import { environment } from '../../environments/environment';
import { UserWithAccounts } from '../_models/UserWithAccounts';

@Injectable()
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  // jwtHelperService = new JwtHelperService();
  userToken: any;
  private _decodedToken: any;
  currentUser: UserWithAccounts;
  defaultPhoto = '../assets/user.png';
  private prefixPhotoPath = '..';
  private photoUrl: BehaviorSubject<string> = new BehaviorSubject<string>(
    this.defaultPhoto
  );
  currentPhotoUrl = this.photoUrl.asObservable();

  private refreshToken() {
    if (!this._decodedToken) {
      this.userToken = localStorage.getItem('token');
      this._decodedToken = this.jwtHelperService.decodeToken(
        this.userToken
      );
    }

  }
  get decodedToken(): any {
    this.refreshToken();
    return this._decodedToken;
  }

  constructor(
    private http: HttpClient,
    private jwtHelperService: JwtHelperService
  ) {}


  changeMemberPhoto(photoUrl: string, pathPrefix = '') {
    if (photoUrl === null) {
      photoUrl = pathPrefix + this.defaultPhoto;
    }
    this.photoUrl.next(photoUrl);
    if (this.currentUser.photoUrl !== photoUrl) {
      this.currentUser.photoUrl = photoUrl;
      localStorage.setItem('user', JSON.stringify(this.currentUser));
    }
  }

  login(model: any) {
    return this.http
      .post<AuthUser>(this.baseUrl + 'login', model, {
        headers: new HttpHeaders().set('Content-Type', 'application/json')
      })
      .pipe(
        map(user => {
          if (user) {
            localStorage.setItem('token', user.tokenString);
            localStorage.setItem('user', JSON.stringify(user.user));
            this._decodedToken = this.jwtHelperService.decodeToken(
              user.tokenString
            );
            this.userToken = user.tokenString; // gebruik ik dat ergens?
            this.currentUser = user.user;
            this.currentUser.fromCache = true;
            this.changeMemberPhoto(this.currentUser.photoUrl, '../');
            console.log(this.decodedToken);
          }
        })
      );
  }

  register(user: User) {
    return this.http.post(this.baseUrl + 'register', user, {
      headers: new HttpHeaders().set('Content-Type', 'application/json')
    });
  }
  updateUserCache(user: UserWithAccounts) {
    this.currentUser = user;
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.fromCache = true;
  }
  clearUserCache() {
    this.currentUser = null;
  }
  loggedIn() {
    this.refreshToken();
    return !this.jwtHelperService.isTokenExpired(this.userToken);
  }
}
