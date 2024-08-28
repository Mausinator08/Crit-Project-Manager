import 'process';

export type DevelopmentLocalEnv = {
    critApiUrl: string;
};

const developmentLocalEnv: DevelopmentLocalEnv = {
    critApiUrl: "https://localhost:7295/api",
};

export function GetEnvValues(): DevelopmentLocalEnv | null {
    if (process.env.NODE_ENV === "development") {
        return developmentLocalEnv;
    }

    return null;
}
