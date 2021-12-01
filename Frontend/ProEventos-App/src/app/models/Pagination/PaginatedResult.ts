import { Pagination } from './Pagination';

export class PaginatedResult<T> {
  constructor(public result: T, public pagination?: Pagination) {}
}
