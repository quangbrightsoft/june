export interface GetUsersModel {
    search: string;
    page: number;
    pageSize: number;
    sortBy: string;
    descending: boolean;
    roles: string[];
}