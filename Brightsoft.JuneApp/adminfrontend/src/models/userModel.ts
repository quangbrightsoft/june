import { BaseModel } from "./baseModel";

export interface UserModel extends BaseModel {
    userName: string;
    ssn: string;
    email: string;
    isDisabled: boolean;
    roles: string[];
}