import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { CookieService } from "ngx-cookie-service";
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

    constructor(private cookieService: CookieService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.cookieService.get('jwtToken');

        if(token) {
            req = req.clone({
                setHeaders: {Authorisation: `Bearer ${token}`}
            });
        }

        return next.handle(req);
    }

}