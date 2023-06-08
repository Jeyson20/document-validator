using System.Text.RegularExpressions;

namespace DocumentValidator
{
	public enum DocumentType
	{
		/// <summary>
		/// Dominican National ID
		/// </summary>
		DNI = 1,

		/// <summary>
		/// Dominican Passport
		/// </summary>
		Passport = 2,

		/// <summary>
		/// Dominican Tax ID
		/// </summary>
		RNC = 3
	}

	public class Validator
	{
		private const string DNI_MULTIPLIERS = "1212121212";
		private const string FIRST_THREE_DIGITS = "000";
		private const int FIRST_TEN_DIGITS = 10;
		private const int DNI_LENGTH = 11;

		private const int PASSPORT_LENGTH = 9;

		private const int RNC_LENGTH = 9;
		private const string RNC_MULTIPLIERS = "79865432";
		private const int FIRST_EIGHT_DIGITS = 8;
		private const int RNC_DIVIDER = 11;
		private const int RNC_VERIFIER_DIGIT = 2;

		public static bool ValidateDocumentNumber(DocumentType documentType, string documentNumber)
		{
			if (string.IsNullOrEmpty(documentNumber)) return false;
			if (documentType == DocumentType.DNI) return IsDniValid(documentNumber);
			if (documentType == DocumentType.Passport) return IsPassportValid(documentNumber);
			if (documentType == DocumentType.RNC) return IsRncValid(documentNumber);
			return true;
		}
		private static bool IsDniValid(string dni)
		{
			if (dni.Length != DNI_LENGTH) return false;

			if (!dni.All(char.IsDigit)) return false;

			if (dni.StartsWith(FIRST_THREE_DIGITS)) return false;

			int[] digits = dni[..FIRST_TEN_DIGITS].Select(c => int.Parse(c.ToString()))
				.ToArray();
			int[] multipliers = DNI_MULTIPLIERS.Select(m => int.Parse(m.ToString()))
				.ToArray();

			int verifierDigit = int.Parse(dni[FIRST_TEN_DIGITS].ToString());

			int sum = digits.Select((digit, index) =>
			{
				int multiplier = multipliers[index];
				int multipliedDigit = digit * multiplier;

				return multipliedDigit < FIRST_TEN_DIGITS
					? multipliedDigit
					: (multipliedDigit / FIRST_TEN_DIGITS) + (multipliedDigit % FIRST_TEN_DIGITS);
			}).Sum();

			int result = (FIRST_TEN_DIGITS - (sum % FIRST_TEN_DIGITS)) % FIRST_TEN_DIGITS;

			return verifierDigit == result;
		}

		private static bool IsPassportValid(string passport)
		{
			string regexPattern = @"^[A-Za-z]{2}\d{7}$";

			if (passport.Length != PASSPORT_LENGTH) return false;

			return Regex.IsMatch(passport, regexPattern);

		}

		private static bool IsRncValid(string rnc)
		{
			if (rnc.Length != RNC_LENGTH) return false;

			if (!rnc.All(char.IsDigit)) return false;

			int[] multipliers = RNC_MULTIPLIERS.Select(m => int.Parse(m.ToString()))
				.ToArray();

			int[] digits = rnc[..FIRST_EIGHT_DIGITS].Select(c => int.Parse(c.ToString()))
				.ToArray();

			//int sum = digits.Zip(multipliers, (digit, multiplier) => digit * multiplier).Sum();

			int sum = digits.Select((digit, index) => multipliers[index] * digit).Sum();

			int verifierDigit = int.Parse(rnc[FIRST_EIGHT_DIGITS].ToString());

			int remainder = sum % RNC_DIVIDER;
			int calculatedVerifierDigit = remainder == 0 ? RNC_VERIFIER_DIGIT : RNC_DIVIDER - remainder;

			return verifierDigit == calculatedVerifierDigit;
		}
	}
}


