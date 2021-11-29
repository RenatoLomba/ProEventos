export interface UserUpdate {
  title: string;
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  function: string;
  description: string | null;
  password: string | null;
  token: string | null;
}
