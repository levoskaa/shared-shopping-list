import { NgModule } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';

const uiModules = [CheckboxModule, InputTextModule, ButtonModule, RippleModule];

@NgModule({
  declarations: [],
  imports: [...uiModules],
  exports: [...uiModules],
})
export class CommonModule {}
