import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BsDropdownModule, TabsModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';
import { RouterModule } from '@angular/router';


import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from '../app/_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
//import { ErrorInterceptorProviders } from '../app/_services/error.interceptor';
import { AlertifyService } from '../app/_services/alertify.service';
import { MemberListComponent } from '../app/members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { appRoutes } from './routes';
import { AuthGuard } from './_guards/auth.guard';
import { environment } from '../environments/environment.prod';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { UserService } from './_services/user.service';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { FileUploadModule } from 'ng2-file-upload';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';

export function tokenGetter() {
    return localStorage.getItem('token');
}

@NgModule({
    declarations: [
        AppComponent,
        NavComponent,
        HomeComponent,
        RegisterComponent,
        MemberListComponent,
        ListsComponent,
        MessagesComponent,
        MemberCardComponent,
        MemberDetailComponent,
        MemberEditComponent,
        PhotoEditorComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        FormsModule,
        TabsModule.forRoot(),
        NgxGalleryModule,
        BsDropdownModule.forRoot(),
        RouterModule.forRoot(appRoutes),
        JwtModule.forRoot({
            config: {
                tokenGetter: tokenGetter,
                whitelistedDomains: [environment.apiDomain],
                blacklistedRoutes: [environment.apiDomain + '/api/auth/']
            }
        }),
        FileUploadModule
    ],
    providers: [
        AuthService,
        // ErrorInterceptorProvider,
        AlertifyService,
        AuthGuard,
        UserService,
        MemberDetailResolver,
        MemberListResolver,
        MemberEditResolver,
        PreventUnsavedChanges
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
