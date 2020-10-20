import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
      let retorno: boolean;
      if (localStorage.getItem('token') !== null) {
        retorno = true;
      } else {
        this.router.navigate(['/user/login']);
        retorno = false;
      }
      return retorno;
  }
}
