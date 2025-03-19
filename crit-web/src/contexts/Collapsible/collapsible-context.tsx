import React, { createContext, useState } from "react";

interface ICollapsibleContext {
    collapsed: boolean;
    toggleAccordion: (event: React.MouseEvent<SVGSVGElement | HTMLElement>) => void;
    setCollapsed: (collapsed: boolean) => void;
}

export const CollapsibleContext = createContext<ICollapsibleContext>({
    collapsed: true,
    toggleAccordion: (event: React.MouseEvent<SVGSVGElement | HTMLElement>) => void {},
    setCollapsed: (collapsed: boolean) => void {}
});

interface CollapsibleProviderProps {
    children?: React.ReactNode;
}

const CollapsibleProvider: React.FC<CollapsibleProviderProps> = ({ children }) => {
    const [collapsed, setCollapsed] = useState<boolean>(true);

    function toggleAccordionHandler(event: React.MouseEvent<SVGSVGElement | HTMLElement>): void {
        if (collapsed === true) {
            setCollapsed(false);
        } else {
            setCollapsed(true);
        }
    };

    return (
        <CollapsibleContext
            value={{
                collapsed: collapsed,
                toggleAccordion: toggleAccordionHandler,
                setCollapsed: setCollapsed,
            }}
        >
            {children}
        </CollapsibleContext>
    );
};

export default CollapsibleProvider;