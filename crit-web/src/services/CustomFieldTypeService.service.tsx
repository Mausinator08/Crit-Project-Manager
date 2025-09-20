import { GetEnvValues } from "../constants/environment";
import { Injectable } from "../functions/Dependencies/injectable";
import { CustomFieldType } from "../models/custom-field-type.model";

@Injectable()
export class CustomFieldTypeService
{
    private readonly customFieldTypeUrl: string =
        GetEnvValues()?.critApiUrl + "/CustomFieldType";

    private constructor() { }

    public async GetAllCustomFieldTypes(projectId: string): Promise<CustomFieldType[]>
    {
        return new Promise<CustomFieldType[]>(async (resolve, reject) =>
        {
            const response = await fetch(new URL(`${this.customFieldTypeUrl}/GetAllCustomFieldTypes/${projectId}`), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok)
            {
                reject(new Error(await response.text()));
                return;
            }

            const customFieldTypes: CustomFieldType[] = await response.json();
            resolve(customFieldTypes);
        });
    }

    public async GetCustomFieldType(customFieldTypeId: string): Promise<CustomFieldType>
    {
        return new Promise<CustomFieldType>(async (resolve, reject) =>
        {
            const response = await fetch(new URL(`${this.customFieldTypeUrl}/${customFieldTypeId}`), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok)
            {
                reject(new Error(await response.text()));
                return;
            }

            const status: CustomFieldType = await response.json();
            resolve(status);
        });
    }

    public async CreateCustomFieldType(customFieldType: CustomFieldType): Promise<CustomFieldType>
    {
        return new Promise<CustomFieldType>(async (resolve, reject) =>
        {
            const response = await fetch(new URL(`${this.customFieldTypeUrl}`), {
                method: "POST",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(customFieldType),
            });

            if (!response.ok)
            {
                reject(new Error(await response.text()));
                return;
            }

            const createdCustomFieldType: CustomFieldType = await response.json();
            resolve(createdCustomFieldType);
        });
    }

    public async UpdateCustomFieldType(customFieldType: CustomFieldType): Promise<CustomFieldType>
    {
        return new Promise<CustomFieldType>(async (resolve, reject) =>
        {
            const response = await fetch(new URL(`${this.customFieldTypeUrl}`), {
                method: "PUT",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(customFieldType),
            });

            if (!response.ok)
            {
                reject(new Error(await response.text()));
                return;
            }

            const updatedCustomFieldType: CustomFieldType = await response.json();
            resolve(updatedCustomFieldType);
        });
    }

    public async DeleteCustomFieldType(customFieldTypeId: string): Promise<void>
    {
        return new Promise<void>(async (resolve, reject) =>
        {
            const response = await fetch(new URL(`${this.customFieldTypeUrl}/${customFieldTypeId}`), {
                method: "DELETE",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok)
            {
                reject(new Error(await response.text()));
                return;
            }

            resolve();
        });
    }
}