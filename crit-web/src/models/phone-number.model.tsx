export class PhoneNumber {
    constructor(type?: number, countryCode?: string, number?: string, extension?: string, organizationId?: string, phoneNumber?: any) {
        this.id = phoneNumber?.id ?? null;
        this.type = type ?? phoneNumber?.type ?? 0;
        this.countryCode = countryCode ?? phoneNumber?.countryCode ?? '';
        this.number = number ?? phoneNumber?.number ?? '';
        this.extension = extension ?? phoneNumber?.extension;
        this.organizationId = organizationId ?? phoneNumber?.organizationId ?? '';
    }

    public id?: string;
    public type: number;
    public countryCode: string;
    public number: string;
    public extension?: string;
    public organizationId: string;
}