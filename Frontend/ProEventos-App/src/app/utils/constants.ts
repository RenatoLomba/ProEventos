import { environment } from '../../environments/environment';

export class Constants {
  static readonly DATE_FMT = 'dd/MM/yyyy';
  static readonly DATE_TIME_FMT = `${Constants.DATE_FMT} hh:mm a`;
  static readonly API_URL = environment.apiUrl + '/api';
  static readonly USER_STORE = '@proeventos/user';
}
