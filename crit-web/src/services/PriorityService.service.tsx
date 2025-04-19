import { GetEnvValues } from "../constants/environment";
import { Injectable } from "../functions/Dependencies/injectable";
import { Priority } from '../models/priority.model';

@Injectable()
export class PriorityService {
    private readonly priorityUrl: string =
        GetEnvValues()?.critApiUrl + "/Priority";

    private constructor() { }

    public async GetAllPriorities(projectId: string): Promise<Priority[]> {
        return new Promise<Priority[]>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.priorityUrl}/GetAllPriorities/${projectId}`), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const priorities: Priority[] = await response.json();
            resolve(priorities);
        });
    }

    public async GetPriority(priorityId: string): Promise<Priority> {
        return new Promise<Priority>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.priorityUrl}/${priorityId}`), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const priority: Priority = await response.json();
            resolve(priority);
        });
    }

    public async CreatePriority(priority: Priority): Promise<Priority> {
        return new Promise<Priority>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.priorityUrl}`), {
                method: "POST",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(priority),
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const createdPriority: Priority = await response.json();
            resolve(createdPriority);
        });
    }

    public async UpdatePriority(priority: Priority): Promise<Priority> {
        return new Promise<Priority>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.priorityUrl}`), {
                method: "PUT",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(priority),
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const updatedPriority: Priority = await response.json();
            resolve(updatedPriority);
        });
    }

    public async DeletePriority(priorityId: string): Promise<void> {
        return new Promise<void>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.priorityUrl}/${priorityId}`), {
                method: "DELETE",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            resolve();
        });
    }
}