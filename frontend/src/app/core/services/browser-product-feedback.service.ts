import { Injectable, signal } from '@angular/core';
import { APP_CONSTANTS } from '../constants/app.constants';
import { IProductFeedback } from './product-feedback.interface';

@Injectable()
export class BrowserProductFeedbackService implements IProductFeedback {
  readonly message = signal('');
  private timer?: ReturnType<typeof setTimeout>;

  async confirmDelete(): Promise<boolean> {
    return window.confirm(APP_CONSTANTS.PRODUCTS.TEXT.CONFIRM_DELETE);
  }

  created(id: number): void {
    window.alert(`${APP_CONSTANTS.PRODUCTS.TEXT.CREATED}${id}`);
  }

  toast(message: string): void {
    clearTimeout(this.timer);
    this.message.set(message);
    this.timer = setTimeout(() => this.message.set(''), APP_CONSTANTS.PRODUCTS.TOAST_DURATION);
  }
}
