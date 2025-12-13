import React, { Context, createContext, useCallback, useRef, useState } from "react";
import { Container } from "../../dependencies/container";
import ErrorPage from "../../pages/Error/error.page";
import { NavLink } from "react-router-dom";
import { Effect } from "../../functions/Utils/effect";

interface ContextProps {
    getService: (service: any) => any;
    id: string;
}

const ModuleContexts: { context: Context<ContextProps>, id: string }[] = [];

export const CreateModuleContext = (id: string): Context<ContextProps> => {
    const ctx = ModuleContexts.find(c => c.id === id)
    if (!ctx) {
        const context = createContext<ContextProps>({ getService: () => { }, id: id });
        ModuleContexts.push({ context, id });
        return context;
    }

    return ctx.context;
};

export const GetModuleContext = (id: string): { context: Context<ContextProps>, id: string } => {
    const ctx = ModuleContexts.find(c => c.id === id);

    if (!ctx) {
        throw new Error(`Module context with id ${id} not found.`);
    }

    return ctx;
}
const containers: Container[] = [];

function CreateContainer(id: string): Container {
    const ct = containers.find(c => c.id === id);
    if (!ct) {
        const container = new Container(id);
        containers.push(container);
        return container;
    }

    return ct;
}

function GetContainer(id: string): Container | undefined {
    return containers.find(c => c.id === id);
}

function UpdateContainer(container: Container): string | null {
    const index = containers.findIndex(c => c.id === container.id);
    if (index !== -1) {
        containers[index] = container;
        return null;
    } else {
        return "Container not found.";
    }
}

interface Props {
    services: any[];
    children?: React.ReactNode;
    id: string;
}

const ModuleProvider: React.FC<Props> = ({ services, children, id }) => {
    const container = useRef<Container>(CreateContainer(id));
    const ModuleContextRef = useRef<Context<ContextProps>>(CreateModuleContext(id));
    const ModuleContext = GetModuleContext(id)?.context ?? ModuleContextRef.current;
    const [isValidContainer, setIsValidContainer] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const initContainer = useCallback(() => {
        let existingContainer = GetContainer(id);
        if (existingContainer && !existingContainer.isInitialized) {
            const error = UpdateContainer(existingContainer.init(services));
            setError(error);
            setIsValidContainer(!error ? true : false);
        } else if (!existingContainer) {
            setIsValidContainer(false);
            setError("Container does not exist.");
        } else {
            setIsValidContainer(true);
            setError(null);
        }
    }, [id, services]);

    const getService = (service: any): any => {
        try {
            return container.current?.get(service);
        } catch (ex: any) {
            return (
                <ErrorPage>
                    <p style={{ color: 'red' }}>{(ex as Error).message}</p>
                    <NavLink to={'/Home'} key='return-home'>
                        Return to Home
                    </NavLink>
                </ErrorPage>
            );
        }
    }

    return (
        <ModuleContext
            value={{
                getService: getService,
                id: id
            }}
            key={`module_context_${id}`}
        >
            <Effect callback={initContainer} key={`effect_${id}`} />
            {error && (
                <ErrorPage>
                    <p style={{ color: 'red' }}>{error}</p>
                    <NavLink to={'/Home'} key='return-home'>
                        Return to Home
                    </NavLink>
                </ErrorPage>
            )}
            {isValidContainer && children}
        </ModuleContext>
    );
};

export default ModuleProvider;
