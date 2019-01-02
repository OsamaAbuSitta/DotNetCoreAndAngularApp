import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(catchError(
            error => {
                if (error.status === 401)
                    return throwError(error.statusText);

                if (error instanceof HttpErrorResponse) {
                    const applicationError = error.headers.get('Application-Error');
                    if (applicationError) {
                        console.error(applicationError);
                        return throwError(applicationError);
                    }
                }

                const serverError = error.error;
                let modalStateErrors = '';
                if (serverError && typeof (serverError) === 'object') {
                    for (const key in serverError) {
                        modalStateErrors += serverError[key] + '\n';
                    }
                }

                return throwError(modalStateErrors || serverError || 'server error');
            }
        ));
    }
}

export const ErrorInterceptorProviders = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
}