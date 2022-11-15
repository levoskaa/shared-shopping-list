export enum DialogCloseType {
  Successful = 'Successful',
  Canceled = 'Canceled',
}

export interface DialogClosedEvent<T = any> {
  closeType: DialogCloseType;
  result?: T;
}
