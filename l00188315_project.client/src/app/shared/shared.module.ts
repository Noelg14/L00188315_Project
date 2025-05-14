import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppTextInputComponent } from './app-text-input/app-text-input.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    AppTextInputComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    AppTextInputComponent
  ]
})
export class SharedModule { }
