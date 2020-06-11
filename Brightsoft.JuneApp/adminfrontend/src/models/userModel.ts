import { BaseModel } from "./baseModel";

export interface UserModel extends BaseModel {
    fullName: string;
    userName: string;
    ssn: string;
    email: string;
    isDisabled: boolean;
    roles: string[];
}