export class ApiResult<T> {
    public message: string | null = null;
    public data: T | null = null;
    public errors: string[] | null = null;
}