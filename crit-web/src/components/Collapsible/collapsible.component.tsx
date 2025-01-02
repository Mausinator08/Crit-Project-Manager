import { JSX } from 'react';

import './collapsible.scss';
import CollapsibleToggle from './collapsible-toggle.component';
import CollapsibleBody from './collapsible-body.component';
import CollapsibleProvider from '../../contexts/Collapsible/collapsible-context';
import CollapsibleLabel from './collapsible-label.component';

type CollapsibleProps = {
    children: React.ReactNode;
};

function Collapsible(props: CollapsibleProps): JSX.Element {
    return (
        <CollapsibleProvider>
            {props.children}
        </CollapsibleProvider>
    );
}

Collapsible.Label = CollapsibleLabel;
Collapsible.Toggle = CollapsibleToggle;
Collapsible.Body = CollapsibleBody;

export default Collapsible;