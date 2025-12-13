import { useEffect } from "react"

interface EffectProps {
    callback: () => void;
};

export function Effect(props: EffectProps) {
    useEffect(() => props.callback?.(), [props]);
    return null;
}