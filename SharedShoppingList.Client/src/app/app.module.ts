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

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
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

@NgModule({
  declarations: [AppComponent, LoginComponent],
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
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
