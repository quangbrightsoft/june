export interface PagedData<T> {
    page: number;
    pageSize: number;
    totalCount: number;
    items: T[];
}