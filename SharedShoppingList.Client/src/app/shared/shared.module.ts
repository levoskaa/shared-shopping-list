import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { InputTextModule } from 'primeng/inputtext';
import { MenuModule } from 'primeng/menu';
import { RippleModule } from 'primeng/ripple';
import { TabViewModule } from 'primeng/tabview';
import { ToastModule } from 'primeng/toast';
import { VirtualScrollerModule } from 'primeng/virtualscroller';
import { HeaderComponent } from './common-ui/header/header.component';
import { LayoutComponent } from './common-ui/layout/layout.component';
import { LogoComponent } from './common-ui/logo/logo.component';

const uiModules = [
  CheckboxModule,
  InputTextModule,
  ButtonModule,
  RippleModule,
  MenuModule,
  ToastModule,
  VirtualScrollerModule,
  DynamicDialogModule,
  TabViewModule,
  ConfirmDialogModule,
  NgScrollbarModule,
];

const components = [LayoutComponent, HeaderComponent, LogoComponent];

@NgModule({
  declarations: [...components],
  imports: [CommonModule, RouterModule, TranslateModule, ...uiModules],
  exports: [CommonModule, TranslateModule, ...uiModules, ...components],
})
export class SharedModule {}
