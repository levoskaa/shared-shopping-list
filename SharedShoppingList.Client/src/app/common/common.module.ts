import { NgModule } from '@angular/core';
import { CheckboxModule } from 'primeng/checkbox';

const uiModules = [CheckboxModule];

@NgModule({
  declarations: [],
  imports: [...uiModules],
  exports: [...uiModules],
})
export class CommonModule {}
