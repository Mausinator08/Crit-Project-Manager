export function objectsEqual(a: any, b: any): boolean {
    if (typeof a !== 'object' || typeof b !== 'object') {
        return false;
    }

    if (a.constructor !== b.constructor) {
        return false;
    }

    let objKeysA = Object.keys(a).sort();
    let objKeysB = Object.keys(b).sort();

    if (objKeysA.length !== objKeysB.length) {
        return false;
    }

    for (let i in objKeysA) {
        if (objKeysA[i] !== objKeysB[i]) {
            return false;
        } else {
            const valueB = b[objKeysB[i]];
            const valueA = a[objKeysA[i]];
            return valuesEqual(valueA, valueB);
        }
    }

    return true;
}

export function valuesEqual(a: any, b: any): boolean {
    switch (typeof a) {
        case 'number':
        case 'boolean':
        case 'string':
            if (a !== b) {
                return false;
            }
            break;
        case 'function':
            if (a.call(a) !== b.call(b)) {
                return false;
            }
            break;
        case 'object':
            if (Array.isArray(a) && Array.isArray(b)) {
                for (let j in a) {
                    return valuesEqual(a[j], b[j]);
                }
            }

            if (a instanceof RegExp || b instanceof RegExp) {
                if (String(a) !== String(b)) {
                    return false;
                } else {
                    return true;
                }
            }

            if (a instanceof Date || b instanceof Date) {
                if (a.valueOf() !== b.valueOf()) {
                    return false;
                } else {
                    return true;
                }
            }

            if (a instanceof Set || b instanceof Set) {
                if (a.size !== b.size) {
                    return false;
                }

                for (let s of a) {
                    if (!b.has(s)) {
                        return false;
                    }
                }

                return true;
            }

            return objectsEqual(a, b);
        default:
            return false;
    }

    return true;
}