import 'reflect-metadata';

export class Container {
    constructor(id: string) {
        this.id = id;
    }

    id: string;
    dependencies: any = [];
    isInitialized: boolean = false;

    public init(deps: any[]) {
        deps.map((target) => {
            const isInjectable = Reflect.getMetadata('injectable', target);
            if (!isInjectable) return;

            // get the typeof parameters of constructor
            const paramTypes = Reflect.getMetadata('design:paramtypes', target) || [];

            // resolve dependecies of current dependency
            const childrenDep = paramTypes.map((paramType: any) => {
                // recursively resolve all child dependencies:
                this.init([paramType]);

                if (!this.dependencies[paramType.name]) {
                    this.dependencies[paramType.name] = new paramType();
                    return this.dependencies[paramType.name];
                }
                return this.dependencies[paramType.name];
            });

            // resolve dependency by injection child classes that already resolved
            if (!this.dependencies[target.name]) {
                this.dependencies[target.name] = new target(...childrenDep);
            }
        });

        this.isInitialized = true;
        return this;
    }

    public get<T extends new (...args: any[]) => any>(
        serviceClass: T,
    ): InstanceType<T> {
        if (this.isInitialized && this.dependencies[serviceClass.name]) {
            return this.dependencies[serviceClass.name];
        }

        throw new Error(`The dependency type "${serviceClass.name}" does not exist. Please make sure the dependency class has been marked with @Injectable() and has been registered in "init(deps).`);
    }
}
