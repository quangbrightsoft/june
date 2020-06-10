import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export abstract class RefreshTokenService 
{    
   /**     
   * Try to authenticate with refresh token and return if auth succeed     
   */    
   abstract tryAuthWithRefreshToken(): Observable<boolean>;
}