import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { environment } from '@environments/environment';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import {
  NgxsStoragePluginModule,
  StorageEngine,
  STORAGE_ENGINE,
} from '@ngxs/storage-plugin';
import { NgxsModule } from '@ngxs/store';

import { MessageService } from 'primeng/api';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JoinUserGroupDialogComponent } from './components/join-user-group-dialog/join-user-group-dialog.component';
import { NewUserGroupDialogComponent } from './components/new-user-group-dialog/new-user-group-dialog.component';
import { ShoppingListComponent } from './components/shopping-list/shopping-list.component';
import { SignInPageComponent } from './components/sign-in-page/sign-in-page.component';
import { SignUpPageComponent } from './components/sign-up-page/sign-up-page.component';
import { UpsertShoppingListEntryDialogComponent } from './components/upsert-shopping-list-entry-dialog/upsert-shopping-list-entry-dialog.component';
import { UserGroupDetailsPageComponent } from './components/user-group-details-page/user-group-details-page.component';
import { UserGroupsPageComponent } from './components/user-groups-page/user-groups-page.component';
import { httpInterceptorProviders } from './core/interceptors';
import { CapitalizeFirstPipe } from './core/pipes/capitalize-first.pipe';
import { SharedModule } from './shared/shared.module';
import { AuthState } from './shared/states/auth/auth.state';

// AoT requires an exported function for factories

export function createTranslateLoader(http: HttpClient): TranslateLoader {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

export function createStorageEngine(): StorageEngine {
  // TODO: add strategy design pattern when prepared to run with Ionic
  return localStorage;
}

const components = [
  AppComponent,
  SignInPageComponent,
  SignUpPageComponent,
  UserGroupsPageComponent,
  UserGroupDetailsPageComponent,
  ShoppingListComponent,
];

const dialogs = [
  NewUserGroupDialogComponent,
  JoinUserGroupDialogComponent,
  UpsertShoppingListEntryDialogComponent,
];

const pipes = [CapitalizeFirstPipe];

@NgModule({
  declarations: [...components, ...pipes, ...dialogs],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    TranslateModule.forRoot({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: createTranslateLoader,
        deps: [HttpClient],
      },
    }),
    NgxsModule.forRoot([AuthState], {
      developmentMode: !environment.production,
    }),
    NgxsStoragePluginModule.forRoot({
      key: [AuthState],
    }),
    NgxsReduxDevtoolsPluginModule.forRoot({
      name: 'Shoppy',
      disabled: environment.production,
    }),
    AppRoutingModule,
    SharedModule,
  ],
  providers: [
    {
      provide: STORAGE_ENGINE,
      useFactory: createStorageEngine,
    },
    httpInterceptorProviders,
    MessageService,
  ],
  entryComponents: [...dialogs],
  bootstrap: [AppComponent],
})
export class AppModule {}
