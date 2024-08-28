import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { icon } from '@fortawesome/fontawesome-svg-core/import.macro';

import NavBar from '../NavBar/nav-bar.component';
import './title-bar.scss';

type Props = {
    onToggleTheme: () => void;
    theme: string;
    onToggleOpen: () => void;
    open: string;
    title: string;
}

function TitleBar(props: Props) {
    return (
        <div id='title-bar' className='grid-container title-bar'>
            <div className='nav-bar'><NavBar open={props.open} onToggleOpen={props.onToggleOpen} /></div>
            <h1 id="title" className='grid-item'>{props.title}</h1>
            <div className='grid-item'>Dark Mode: {props.theme === 'light' ? (<span>Off <FontAwesomeIcon icon={icon({ name: 'toggle-off' })} onClick={props.onToggleTheme} /></span>) : (<span>On <FontAwesomeIcon icon={icon({ name: 'toggle-on' })} onClick={props.onToggleTheme} /></span>)}</div>
        </div>
    );
}

export default TitleBar;