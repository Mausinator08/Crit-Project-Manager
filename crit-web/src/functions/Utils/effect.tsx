import { useEffect } from "react"

interface EffectProps {
    callback: () => void;
};

export function Effect(props: EffectProps) {
    useEffect(() => props.callback?.(), []);
    return null;
}