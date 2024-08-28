import { icon } from "@fortawesome/fontawesome-svg-core/import.macro";
import { IconDefinition } from "@fortawesome/fontawesome-svg-core";
import { IndexRouteObject, NonIndexRouteObject, RouteObject } from "react-router-dom";

import Home from "../pages/Home/home.page";

export type Link = { title: string, path: string, icon: IconDefinition, element: JSX.Element, children?: Link[], index?: boolean };

export const links: Link[] = [
    {
        title: 'Home',
        path: "/",
        icon: icon({ name: 'house' }),
        element: (<Home />),
    }
];

export function LinkChildren(link: Link): RouteObject {
    if (link.index === true) {
        return {
            path: link.path,
            element: link.element,
            children: link.children && link.children.length > 0 ? link.children.map<RouteObject>(LinkChildren) : undefined,
            index: true,
        } as IndexRouteObject;
    }

    return {
        path: link.path,
        element: link.element,
        children: link.children && link.children.length > 0 ? link.children.map<RouteObject>(LinkChildren) : undefined,
    } as NonIndexRouteObject;
}
