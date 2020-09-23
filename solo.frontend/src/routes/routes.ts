export const routes = {
  parkList: "/parks",
  parkDetails: "/parks/:parkId",
  parkObjectDetails: "/parks/:parkId/objects/:objectId",
};

export const buildUrl = (route: string, ...params: (string | number)[]) => {
  const routeParamPlaceholders = route
    .split("/")
    .filter((p) => p.startsWith(":"));

  if (!routeParamPlaceholders.length) {
    return route;
  }

  if (routeParamPlaceholders.length !== params.length) {
    throw Error(
      `Not all parameters are provided! Expected ${routeParamPlaceholders.length} arguments but got ${params.length}. Route: ${route}.`
    );
  }

  let i = 0;
  return route.replace(new RegExp(routeParamPlaceholders.join("|"), "gi"), () =>
    params[i++].toString()
  );
};

export const buildAbsoluteUrl = (
  route: string,
  ...params: (string | number)[]
) => {
  return window.location.origin + buildUrl(route, ...params);
};

export type BuildUrl = (
  route: string,
  ...params: (string | number)[]
) => string;
