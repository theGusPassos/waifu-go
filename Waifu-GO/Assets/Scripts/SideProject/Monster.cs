using UnityEngine;
using WaifuGO.MapDisplay;

namespace WaifuGo.SideProject
{
    public class Monster : MonoBehaviour
    {
        public MonsterType monsterType;
        public TileManager tileManager;

        private float lat = 0f;
        private float lon = 0f;

        public bool testpos = false;
        public float lat_comp, lon_comp;

        public void Init(float _lat, float _lon)
        {
            lat = _lat;
            lon = _lon;

            Debug.Log(lat);
            Debug.Log(lon);
            
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            float x;
            float y;

            Vector3 position = Vector3.zero;

            geodeticOffsetInv(
            tileManager.GetLatitute * Mathf.Deg2Rad,
            tileManager.GetLongitute * Mathf.Deg2Rad,
            lat * Mathf.Deg2Rad,
            lon * Mathf.Deg2Rad, out x, out y);

            if (((lat - tileManager.GetLatitute) < 0 && (lon - tileManager.GetLongitute) > 0) || 
                ((lat - tileManager.GetLatitute) > 0 && (lon - tileManager.GetLongitute) < 0))
            {
                position = new Vector3(x, 0, y);
            }
            else
            {
                position = new Vector3(-x, 0, -y);
            }

            transform.position = position;

            position.x *= 0.3000122f;
            position.z *= 0.123043f;
        }

        private void Update()
        {
            if (testpos)
            {
                //seta manualmento a posição
                Init(lat_comp, lon_comp);
            }
        }

        private void geodeticOffsetInv(float refLat, float refLon,
                float lat, float lon,
                out float xOffset, out float yOffset)
        {

            float GD_semiMajorAxis = 6378137.000000f;
            float GD_TranMercB = 6356752.314245f;
            float GD_geocentF = 0.003352810664f;

            float a = GD_semiMajorAxis;
            float b = GD_TranMercB;
            float f = GD_geocentF;

            float L = lon - refLon;
            float U1 = Mathf.Atan((1 - f) * Mathf.Tan(refLat));
            float U2 = Mathf.Atan((1 - f) * Mathf.Tan(lat));
            float sinU1 = Mathf.Sin(U1);
            float cosU1 = Mathf.Cos(U1);
            float sinU2 = Mathf.Sin(U2);
            float cosU2 = Mathf.Cos(U2);

            float lambda = L;
            float lambdaP;
            float sinSigma;
            float sigma;
            float cosSigma;
            float cosSqAlpha;
            float cos2SigmaM;
            float sinLambda;
            float cosLambda;
            float sinAlpha;
            int iterLimit = 100;
            do
            {
                sinLambda = Mathf.Sin(lambda);
                cosLambda = Mathf.Cos(lambda);
                sinSigma = Mathf.Sqrt((cosU2 * sinLambda) * (cosU2 * sinLambda) +
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) *
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
                if (sinSigma == 0)
                {
                    xOffset = 0.0f;
                    yOffset = 0.0f;
                    return;  // co-incident points
                }
                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                sigma = Mathf.Atan2(sinSigma, cosSigma);
                sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;
                cosSqAlpha = 1 - sinAlpha * sinAlpha;
                cos2SigmaM = cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha;
                if (cos2SigmaM != cos2SigmaM) //isNaN
                {
                    cos2SigmaM = 0;  // equatorial line: cosSqAlpha=0 (ยง6)
                }
                float C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));
                lambdaP = lambda;
                lambda = L + (1 - C) * f * sinAlpha *
                    (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            } while (Mathf.Abs(lambda - lambdaP) > 1e-12 && --iterLimit > 0);

            if (iterLimit == 0)
            {
                xOffset = 0.0f;
                yOffset = 0.0f;
                return;  // formula failed to converge
            }

            float uSq = cosSqAlpha * (a * a - b * b) / (b * b);
            float A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            float B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
            float deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) -
                B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));
            float s = b * A * (sigma - deltaSigma);

            float bearing = Mathf.Atan2(cosU2 * sinLambda, cosU1 * sinU2 - sinU1 * cosU2 * cosLambda);
            xOffset = Mathf.Sin(bearing) * s;
            yOffset = Mathf.Cos(bearing) * s;
        }
    }

    public enum MonsterType
    {
        IceSlime,
        FireSlime,
        EarthSlime
    }
}

