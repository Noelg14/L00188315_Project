import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './app-text-input.component.html',
  styleUrls: ['./app-text-input.component.scss']
})
export class AppTextInputComponent implements ControlValueAccessor {

  @Input() type = 'text';
  @Input() label = '';

  /**
   *
   */
  constructor(@Self() public controlDir: NgControl) {
    this.controlDir.valueAccessor = this;
  }

  writeValue(obj: any): void {}
  registerOnChange(fn: any): void {}
  registerOnTouched(fn: any): void {}

  get control(): FormControl {
    return this.controlDir.control as FormControl;
  }


}
