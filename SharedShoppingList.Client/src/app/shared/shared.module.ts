import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { LogoComponent } from './common-ui/logo/logo.component';

const uiModules = [CheckboxModule, InputTextModule, ButtonModule, RippleModule];

const components = [LogoComponent];

@NgModule({
  declarations: [...components],
  imports: [...uiModules],
  exports: [CommonModule, TranslateModule, ...uiModules, ...components],
})
export class SharedModule {}
